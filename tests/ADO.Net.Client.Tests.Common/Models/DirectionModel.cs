using ADO.Net.Client.Annotations;

namespace ADO.Net.Client.Tests.Common.Models
{
    public class DirectionModel
    {
        [Output]
        public object Output { get; set; }
        [ReturnValue]
        public object ReturnValue { get; set; }
        [Input]
        public object Input { get; set; }
        [InputOutput]
        public object InputOutput { get; set; }
        public object NoDirection { get; set; }
    }
}