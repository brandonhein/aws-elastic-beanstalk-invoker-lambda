# aws-elastic-beanstalk-invoker-lambda

### Background (the why)
There are instances where an Application needs to be invoked periodically.  We achieve this in software using [Cron-jobs](https://en.wikipedia.org/wiki/Cron).  I like to use [Quartz](https://www.quartz-scheduler.net) to schedule code to run for my .NET applications.  

With the transition of moving applications to the AWS cloud (Elastic Beanstalk application specifically), using Quartz worked for me, but it was executing the same code, times the number of instances I had present (for load balancing).  Basically something I wanted executed once, was getting executed 4 other times.  This created undesired duplicate records.

I also had a similar scenario with reading messages from an SQS queue.  Where I really just wanted one instance to read messages from the queue vs a bunch of instances potentially getting/executing on the same message.

Elastic Beanstalk has a 'worker' server you can use, which is ideally what it's designed to solve.  One instance, to run your long running processes to keep your 'web' server available for other calls.  Unfortunately, these 'worker' servers aren't available for .NET code, and being primarily a .NET developer, this was thrown away as a solution for me as I didn't have the time to learn another language.

My solution for the SQS message listener was to create a message 'receiver' controller for my EB app.  And a Lambda function that was triggered by SQS.  The function forwarded on the message to this controller via an HTTP post (like an SNS [[another topic with SNS vs EB behind a private VPC]](http://localhost)).  And that worked beautifully.  The load balancer found one instance for me, it executed the code once, and the lambda function auto acks the message from the queue like magic. 

After looking up ways to do AWS Cron Jobs, I stumbled across this [article](https://medium.com/blogfoster-engineering/running-cron-jobs-on-aws-lambda-with-scheduled-events-e8fe38686e20).  Leveraging CloudWatch and Lambda, I knew I build out a similar solution like I did for my SQS message receiver.  Call the controller, on the CloudWatch schedule that I configure.
