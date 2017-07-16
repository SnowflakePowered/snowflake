const path = require("path");
var webpackConfig = require("./webpack.config");
webpackConfig.entry = {};

module.exports = function (config) {
  config.set({
    basePath: "",
    frameworks: ["mocha", "chai"],
    files: [
      "node_modules/babel-polyfill/dist/polyfill.js",
      "src/tests/*.ts",
    ],
    exclude: [
    ],
    preprocessors: {
      "src/tests/*.ts": ["webpack"],
    },
    webpack: {
      module: {
        rules: [{
          test: /\.tsx?$/,
          exclude: path.resolve(__dirname, "node_modules"),
          include: path.resolve(__dirname, "src"),
          use: [
            {
              loader: "babel-loader",
              query: {
                presets: ["es2015"]
              },
            },
            {
              loader: "ts-loader",
            }],
        }],
      },
      resolve: webpackConfig.resolve,
    },
    reporters: ["mocha"],
    port: 9876,
    colors: true,
    logLevel: config.LOG_INFO,
    autoWatch: true,
    browsers: ["PhantomJS"],
    singleRun: true,
    concurrency: Infinity,
  });
};