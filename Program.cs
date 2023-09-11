namespace BlobStorage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Azure;
using NHapi.Model.V23.Datatype;
using NHapi.Model.V23.Message;
using NHapi.Base.Parser;

class Program
{
    static void Main(string[] args)
    {
        var parser = new PipeParser();
        var connectionString = "DefaultEndpointsProtocol=https;AccountName=nhapi;AccountKey=UkFUH4QZR6TQvr+IugS1slIpke0VJzmOsscKyFgtFvYEalLvGUwmomb/diCL8VSCFa/3D29VSBcs+ASt81gOwQ==;EndpointSuffix=core.windows.net";
        var BlobServiceClient = new BlobServiceClient(connectionString);
        var ContainerClient = BlobServiceClient.GetBlobContainerClient("nhapi");

        var _oruR01Message = CreateORU_R01_Message();

        var path = "";
        var submitter = "radiology";
        var someID = "slkafjlkjsldjk";

        if(submitter == "radiology")
        {
            path = $"radiology/path/{someID}";
        }
        else
        {
            path = $"nonRadiology";
        }

        ContainerClient.UploadBlob(path, BinaryData.FromString(parser.Encode(_oruR01Message)));
    }

    public static ORU_R01 CreateORU_R01_Message()
    {
        var _oruR01Message = new ORU_R01();

        var mshSegment = _oruR01Message.MSH;
        mshSegment.FieldSeparator.Value = "|";
        mshSegment.EncodingCharacters.Value = "^~\\&";
        mshSegment.SendingApplication.NamespaceID.Value = "Our System";
        mshSegment.SendingFacility.NamespaceID.Value = "Our Facility";
        mshSegment.ReceivingApplication.NamespaceID.Value = "Their Remote System";
        mshSegment.ReceivingFacility.NamespaceID.Value = "Their Remote Facility";
        mshSegment.MessageType.MessageType.Value = "ORU";
        mshSegment.MessageType.TriggerEvent.Value = "R01";
        mshSegment.VersionID.Value = "2.3";
        mshSegment.ProcessingID.ProcessingID.Value = "P";

        return _oruR01Message;
    }
}
