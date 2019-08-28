'use strict'
var http = require('./http-helper');


exports.handler = async (event, context) => {
    var url = process.env.url;
    var method = process.env.method;
    var body = process.env.body;

    var result;
    if (method.toLowerCase() == "post") {
        result = await http.post(url, body);
    }
    else {
        result = await http.get(url);
    }

    return result;
}