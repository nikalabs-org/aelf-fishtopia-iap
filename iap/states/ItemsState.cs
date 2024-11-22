
using System.Collections.Generic;
using AElf.Sdk.CSharp.State;
using AElf.Types;

namespace AElf.Contracts.FishtopiaIAP
{
    public partial class FishtopiaIAPState
    {
        public MappedState<string, ItemsDAO> ItemsList { get; set; }
    }
}