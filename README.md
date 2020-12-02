# Product identification system based on Microsoft Azure services

Full build [![Full Build Status](https://dev.azure.com/michalchecinski/product%20indetification/_apis/build/status/master_full?branchName=master)](https://dev.azure.com/michalchecinski/product%20indetification/_build/latest?definitionId=8&branchName=master)

Frontend build [![Frontend Build Status](https://dev.azure.com/michalchecinski/product%20indetification/_apis/build/status/master-frontend?branchName=master)](https://dev.azure.com/michalchecinski/product%20indetification/_build/latest?definitionId=11&branchName=master)

## System use case

Product identification system is meant to recognize products based on the images and add appropriate product to virtual shopping cart and receipt.

This system is meant to replace self-service checkout, that scans barcodes of products. Instead, people can make photos of the products that they are putting in the cart in the store, and don't need to worry about looking for barcode on product, making photo in a right angle etc. Customer need to just snap a photo of the product, and then machine learning model will recognize the product itself, and add it to the customers receipt.

### Video demo

[Product Identification - self-service checkout based on images detection (Azure Custom Vision) - YouTube](https://www.youtube.com/watch?v=Z0kb-IF13vo&list=PLPZAYWSrZqficrP4EO-EOTYEPvGO6mECd&index=3)

## Architecture

![Azure architecture for solution](https://github.com/michalchecinski/product-identification/blob/master/images/arch.png)

## Reproduce this solution

This solution has ARM templates to automate Azure deployments. The templates are located in [the folder in this soultion](https://github.com/michalchecinski/product-identification/tree/master/src/ProductIdentification.AzureRG).

There are three main [build pipelines](https://dev.azure.com/michalchecinski/product indetification/_build):

- [master_full](https://dev.azure.com/michalchecinski/product indetification/_build?definitionId=8) - to build the management portal
- [master_infrastructure](https://dev.azure.com/michalchecinski/product indetification/_build?definitionId=12) - to copy infrastructure ARM templates to artifact
- [master-frontend](https://dev.azure.com/michalchecinski/product indetification/_build?definitionId=11) - to build frontend app build on React.js

### Links

There is another repo which is hosting frontend webapp written in React.js. It can be found here: [michalchecinski/product-identification-front](https://github.com/michalchecinski/product-identification-front).
