using AElf.Contracts.MultiToken;
using AElf.Sdk.CSharp;
using AElf.Types;
using Google.Protobuf.WellKnownTypes;

namespace AElf.Contracts.FishtopiaIAP
{
    // Contract class must inherit the base class generated from the proto file
    public class FishtopiaIAP : FishtopiaIAPContainer.FishtopiaIAPBase
    {
        private const string ReceiverAddress = "TiDZhLij1qPtR95AZbQMVtydx436DigRea4w1Vs914StaQUTJ";

        private const string _symbol = "ELF";

        public override Empty Initialize(Empty input)
        {
            Assert(State.Initialized.Value == false, "Already initialized.");

            State.Initialized.Value = true;
            State.Owner.Value = Context.Sender;
            State.ReceiverWalletAddress.Value = Address.FromBase58(ReceiverAddress);
            State.TokenContract.Value = Context.GetContractAddressByName(SmartContractConstants.TokenContractSystemName);

            return new Empty();
        }

        public override BoolValue IsInitialized(Empty input)
        {
            return new BoolValue
            {
                Value = State.Initialized.Value
            };
        }

        public override Empty SetReceiverWallet(AddressInput input)
        {
            AssertIsOwner();
            State.ReceiverWalletAddress.Value = input.Address;
            return new Empty();
        }

        public override Empty AddItems(AddItemsInput input)
        {
            AssertIsOwner();
            Assert(State.ItemsList[input.ItemsId] == null, "Items Already Exists.");

            ItemsDAO items = new()
            {
                ItemsId = input.ItemsId,
                IsAvailable = input.IsAvailable,
                CanBuy = input.CanBuy,
                ItemsPrice = input.ItemsPrice,
            };

            State.ItemsList[input.ItemsId] = items;

            Context.Fire(new AddedItemsEvent
            {
                ItemsId = input.ItemsId,
                IsAvailable = input.IsAvailable,
                CanBuy = input.CanBuy,
                ItemsPrice = input.ItemsPrice,
            });

            return new Empty();
        }

        public override Empty RemoveItems(RemoveItemsInput input)
        {
            AssertIsOwner();
            Assert(State.ItemsList[input.ItemsId] != null, "Items Not Found");
            State.ItemsList.Remove(input.ItemsId);

            Context.Fire(new RemovedItemsEvent
            {
                ItemsId = input.ItemsId,
            });

            return new Empty();
        }

        public override BoolValue IsItemsAvailable(IsItemsAvailableInput input)
        {
            ItemsDAO items = State.ItemsList[input.ItemsId];
            if (items == null || items.IsAvailable == false) return new BoolValue { Value = false };
            return new BoolValue { Value = true };
        }

        public override Empty TransferOwnership(AddressInput input)
        {
            AssertIsOwner();
            Assert(State.Owner.Value != input.Address, "Current Address Is The Owner.");
            State.Owner.Value = input.Address;
            return new Empty();
        }

        public override Empty PurchaseItems(PurchaseItemsInput input)
        {
            ItemsDAO items = State.ItemsList[input.ItemsId];
            Assert(items != null, "Items Not Found.");
            Assert(items.IsAvailable != false, "Items Is Not Available.");
            Assert(items.CanBuy != false, "Items Can Not Buy Now. Please try again.");

            State.TokenContract.TransferFrom.Send(new TransferFromInput
            {
                From = Context.Sender,
                To = State.ReceiverWalletAddress.Value,
                Symbol = _symbol,
                Amount = items.ItemsPrice,
            });

            Context.Fire(new PurchaseItemsEvent
            {
                UserId = input.UserId,
                ItemsId = input.ItemsId,
            });
            return new Empty();
        }

        public override StringValue Owner(Empty input)
        {
            return new StringValue { Value = State.Owner.Value.ToBase58() };
        }

        public override StringValue ReceiverWallet(Empty input)
        {
            return State.ReceiverWalletAddress.Value == null ? new StringValue() : new StringValue { Value = State.ReceiverWalletAddress.Value.ToBase58() };
        }

        public override ItemsPriceOutput GetItemsPrice(GetItemsPriceInput input)
        {
            ItemsPriceOutput itemsPriceOutput = new ItemsPriceOutput();
            foreach (string items in input.ItemsIds)
            {
                ItemsDAO itemExisted = State.ItemsList[items];
                if (items == null) continue;
                itemsPriceOutput.ItemsOutput.Add(itemExisted);
            }
            return itemsPriceOutput;
        }

        private void AssertIsOwner()
        {
            Assert(Context.Sender == State.Owner.Value, "Unauthorized To Perform The Action.");
        }
    }
}