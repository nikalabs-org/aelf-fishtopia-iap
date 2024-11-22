using AElf.Sdk.CSharp.State;
using AElf.Types;

namespace AElf.Contracts.FishtopiaIAP
{
    public partial class FishtopiaIAPState : ContractState
    {
        public BoolState Initialized { get; set; }
        public SingletonState<Address> ReceiverWalletAddress { get; set; }
        public SingletonState<Address> Owner { get; set; }
    }
}