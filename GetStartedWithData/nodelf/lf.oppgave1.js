var http = require("http");

/**
 * getJSON:  REST get request returning JSON object(s)
 * @param options: http options object
 * @param callback: callback to pass the results JSON object(s) back
 */
var getJSON = function(options, onResult)
{
    console.log("rest::getJSON");

    var req = http.request(options, function(res)
    {
        var output = '';
        console.log(options.host + ':' + res.statusCode);
        res.setEncoding('utf8');

        res.on('data', function (chunk) {
            output += chunk;
        });

        res.on('end', function() {
            var obj = JSON.parse(output);
            onResult(res.statusCode, obj);
        });
    });

    req.on('error', function(err) {
        console.log(err)
        onResult(500, err);
    });

    req.end();
};


exports.get = function(request, response) {
    
    console.log(request.params.search);
    var options = {
        host: 'www.prisguide.no',
        path: '/service?module=PGSolrSearch&service=liveSearch&q=' + request.query.search,
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    };
    
    getJSON(options, function(statusCode, result) {
        if(statusCode == statusCodes.OK) {
            var product = {};
            if(result.hits) {
                var firstProduct = result.products[0];
                product.title = firstProduct.title;
                product.thumbnailUrl = firstProduct.thumbnail_url;
                product.imageUrl = firstProduct.image_url;
            }
            response.send(statusCodes.OK, product);
            
        } else {
            response.send(statusCode, { message: 'error'});
        }
          
    });  
        
};