var reqPromise = require("request-promise");

module.exports = {
  post: function(url, request) {
    var options = {
      method: "POST",
      uri: url,
      body: request,
      time: true,
      rejectUnauthorized: false,
      resolveWithFullResponse: true
    };

    var results = reqPromise(options)
      .then(function(responseBody) {
        return responseBody;
      })
      .catch(function(err){
        throw err;
      });

      return results;
  },

  get: function(url) {
    var options = {
      method: "GET",
      uri: url,
      json: true,
      time: true,
      rejectUnauthorized: false,
    };

    var results = reqPromise(options)
      .then(function(res) {
        return res;
      })
      .catch(function(err){
        return err;
      });
    return results;
  }
};