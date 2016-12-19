'use strict';

var path = require('path');

module.exports = {
  cache: true,
  entry: {
    main: './src/index.tsx',
    vendor: [
      'react',
      'react-dom'
    ]
  },
  output: {
    path: path.resolve(__dirname, './dist'),
    filename: '[name].js',
    chunkFilename: '[chunkhash].js'
  },
  module: {
    loaders: [{
      test: /\.ts(x?)$/,
      exclude: "./node_modules",
      loader: 'awesome-typescript-loader'
    }]
  },
  plugins: [
  ],
  resolve: {
    extensions: ['.ts', '.tsx', '.js']
  },
  externals: {
    "react": "React",
    "react-dom": "ReactDOM"
  }
};