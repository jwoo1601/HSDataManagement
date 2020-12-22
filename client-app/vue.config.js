module.exports = {
  outputDir: "../wwwroot/app",
  devServer: {
    proxy: {
      "^/": {
        target: "http://localhost:5000",
        changeOrigin: true,
      },
    },
  },
  configureWebpack: {
    devtool: "source-map",
  },
};
