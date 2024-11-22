using AElf.Cryptography.ECDSA;
using AElf.Testing.TestBase;

namespace AElf.Contracts.FishtopiaIAP
{
    // The Module class load the context required for unit testing
    public class Module : ContractTestModule<FishtopiaIAP>
    {
        
    }
    
    // The TestBase class inherit ContractTestBase class, it defines Stub classes and gets instances required for unit testing
    public class TestBase : ContractTestBase<Module>
    {
        // The Stub class for unit testing
        internal readonly FishtopiaIAPContainer.FishtopiaIAPStub FishtopiaIAPStub;
        // A key pair that can be used to interact with the contract instance
        private ECKeyPair DefaultKeyPair => Accounts[0].KeyPair;

        public TestBase()
        {
            FishtopiaIAPStub = GetFishtopiaIAPContractStub(DefaultKeyPair);
        }

        private FishtopiaIAPContainer.FishtopiaIAPStub GetFishtopiaIAPContractStub(ECKeyPair senderKeyPair)
        {
            return GetTester<FishtopiaIAPContainer.FishtopiaIAPStub>(ContractAddress, senderKeyPair);
        }
    }
    
}