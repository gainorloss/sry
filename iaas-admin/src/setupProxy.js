const { createProxyMiddleware } = require('http-proxy-middleware');
console.log(`proxy:${process.env.proxy_url}`);
module.exports = function (app) {
  app.use(
    '/api',
    createProxyMiddleware({
      target: process.env.proxy_url,
      changeOrigin: true,
      pathRewrite: {
        '^/api': ''
      }
    })
  );
};