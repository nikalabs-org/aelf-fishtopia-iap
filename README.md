# aelf-smc



## Getting started

To make it easy for you to deploy a aelf smart contract, here's a list of recommended next steps.


## Setting up development environment

- Basic knowledge of terminal commands
- IDE - Install VS Code
- [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](#Install dotnet 8.0.x SDK)
- Install aelf contract templates:
```
dotnet new --install AElf.ContractTemplates
```
AELF.ContractTemplates contains various predefined templates for the ease of developing smart contracts on the aelf blockchain.

- Install aelf deploy tool:
```
dotnet tool install --global aelf.deploy
```
aelf.deploy is a utility tool for deploying smart contracts on the aelf blockchain. Please remember to export PATH after installing aelf.deploy.

- Install Node.js and Yarn.
- Install aelf-command:
```
sudo npm i -g aelf-command
```

## Develop Smart Contract
- Cd to src
- Build smart contract:
```
dotnet build
```

## Deploy Smart Contract
- Create A Wallet. To send transactions on the aelf blockchain, you must have a wallet.
```
aelf-command create
```

- Export your wallet password as shown below:
```
export WALLET_PASSWORD="YOUR_WALLET_PASSWORD"
```
Acquire Testnet Tokens (Faucet) for Development: To deploy smart contracts or execute on-chain transactions on aelf, you'll require testnet ELF tokens.
Get ELF Tokens
Go to https://faucet-ui.aelf.dev Enter your address and click Get Tokens.

Run the following command to deploy a contract. Remember to export the path of HelloWorld.dll.patched to CONTRACT_PATH:
```
export CONTRACT_PATH=$(find ~+ . -path "*patched*" | head -n 1)
```

- Deploy Smart Contract:
```
aelf-deploy -a $WALLET_ADDRESS -p $WALLET_PASSWORD -c $CONTRACT_PATH -e https://tdvw-test-node.aelf.io/
```