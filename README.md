# aws-elastic-beanstalk-invoker-lambda

### Background (the why)
There are instances where an Application needs to be invoked periodically.  We achieve this in software using [Cron-jobs](https://en.wikipedia.org/wiki/Cron).  I like to use [Quartz](https://www.quartz-scheduler.net) to schedule code to run for my .NET applications.  

With the transition of moving applications to the AWS cloud (Elastic Beanstalk application specifically), using Quartz worked for me, but it was executing the same code, times the number of instances I had present (for load balancing).  Basically something I wanted executed once, was getting executed 4 other times.  This created undesired duplicate records.

I also had a similar scenario with reading messages from an SQS queue.  Where I really just wanted one instance to read messages from the queue vs a bunch of instances potentially getting/executing on the same message.

Elastic Beanstalk has a 'worker' server you can use, which is ideally what it's designed to solve.  One instance, to run your long running processes to keep your 'web' server available for other calls.  Unfortunately, these 'worker' servers aren't available for .NET code, and being primarily a .NET developer, this was thrown away as a solution for me as I didn't have the time to learn another language.

My solution for the SQS message listener was to create a message 'receiver' controller for my EB app.  And a Lambda function that was triggered by SQS.  The function forwarded on the message to this controller via an HTTP post (like an SNS [[another topic with SNS vs EB behind a private VPC]](http://localhost)).  And that worked beautifully.  The load balancer found one instance for me, it executed the code once, and the lambda function auto acks the message from the queue like magic. 

After looking up ways to do AWS Cron Jobs, I stumbled across this [article](https://medium.com/blogfoster-engineering/running-cron-jobs-on-aws-lambda-with-scheduled-events-e8fe38686e20).  Leveraging CloudWatch and Lambda, I knew I build out a similar solution like I did for my SQS message receiver.  Call the controller, on the CloudWatch schedule that I configure.


### Creation/Setup
#### Lambda steps
1. Create a Lambda Function in the region that the Elastic Beanstalk app lives, that has the `AWSLambdaBasicExecutionRole`.
2. Runtime is `.NET Core 2.1 (C#/PowerShell)
3. Handler `ElasticBeanstalk.Invoker.Lambda::ElasticBeanstalk.Invoker.Lambda.Function::Handler`
3. If your Elastic Beanstalk app is behind private VPC follow the next steps... if not you can skip to step 6.
4. Make sure you also add the `AWSLambdaVPCAccessExecutionRole` role to your lambda
5. Setup your Network for the lambda to register being part of your private VPC.  (correct subnets)
6. Add a `url` enviornment variable with the url to your EB app that should be invoked.  example: `http://my-eb-app.us-region-1.elasticbeanstalk.com/invokeme`
7. Add a `method` enviornment variable with the http method that will be invoking the url from step 4. example: `POST`
8. optional: Add a `body` enviornment variable with the body you want to use to send to the url from step 4.
9. Upload/deploy the lambda deployment zip
10. Hit save

#### CloudWatch steps 
*dependant on the lambda being created*
1. Go to CloudWatch, on the left under Events, click on Rules
2. Create rule
3. Check Schedule, and create a cron of when you want your EB app invoked
4. Add a target and select your lambda function you created
5. optional: if you want to pass in a constant json text vs having one set in your environment variable for the lambda, this is where you set it in the configure input
6. Click on Configure Details and create a name for this job and a description to help others know what is doing. Make sure enabled is checked
7. Hit save and youre all done
