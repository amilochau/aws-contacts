<p align="center">
  <a href="https://contact.milochau.com" target="_blank">
    <img alt="aws-contacts logo" width="100" src="./src/contacts-client/src/assets/logo.png">
  </a>
</p>
<p align="center">
  <a href="https://github.com/vuetifyjs/vuetify/blob/master/LICENSE.md">
    <img src="https://img.shields.io/github/license/amilochau/aws-contacts" alt="License">
  </a>
</p>
<h1 align="center">
  aws-contacts
</h1>

`aws-contacts` is a website used to send and storage contact messages.

## Main features

- UI & API to let users send messages
- Scheduled process to send daily pending messages to administrators

## Usage

To use `aws-contacts` in your own organization, you have to adapt it.

1. Fork the current repository
2. Adapt the organization-specific settings to your own organization
3. Deploy it

The following files have to be adapted.

| File | Adaptations needed | Comment |
| ---- | ------------------ | ------- |
| [CD - deploy settings](./.github/workflows/deploy.yml) | Change the `INFRA_AWS_ROLE`, `INFRA_AWS_REGION` settings |
| [IaC - dev settings](./.tf/hosts/dev.tfvars) | Change the `conventions` and `domains` |
| [IaC - prd settings](./.tf/hosts/prd.tfvars) | Change the `conventions` and `domains` |
| [API settings](./src/contacts-client/src/data/config.ts) | Change the environment-specific API settings |

## Underlying technologies

`aws-contacts` uses the following technologies to work:

- Front-End (UI client): `vue.js v3`, `vuetify`
- Back-End (Functions): `.NET 7.0 native AOT`
- Infrastructure: `AWS CloudFront`, `AWS Lambda`, `AWS S3`, `AWS DynamoDB`
- DevOps: `GitHub Actions`, `Terraform`

The following `amilochau` packages are used:

- [amilochau/core-aws](https://github.com/amilochau/core-aws): AWS Lambda, AWS DynamoDB helpers
- [amilochau/core-vue3](https://github.com/amilochau/core-vue3): vue.js v3 layout
- [amilochau/github-actions](https://github.com/amilochau/github-actions): GitHub Actions
- [amilochau/tf-modules](https://github.com/amilochau/tf-modules): Terraform modules

--- 

## Contribute

Feel free to push your code if you agree with publishing under the [MIT license](./LICENSE).
