using DocumentsService.Protos;
using Grpc.Core;

public class DocumentServiceImpl : DocumentService.DocumentServiceBase
{
  private static readonly List<Document> _documents = new List<Document>()
  {
    new Document { Id = "1", PatientId = "123", Name = "Document1" },
    new Document { Id = "2", PatientId = "456", Name = "Document2" },
  };

  public override Task<DocumentList> GetAll(PatientId request, ServerCallContext context)
  {
    var documentList = new DocumentList();
    documentList.Documents.AddRange(_documents.Where(d => d.PatientId == request.Id));
    return Task.FromResult(documentList);
  }

  public override Task<Document> Get(DocumentId request, ServerCallContext context)
  {
    var document = _documents.FirstOrDefault(d => d.Id == request.Id);
    if (document == null)
    {
      throw new RpcException(new Status(StatusCode.NotFound, $"Document with ID {request.Id} not found."));
    }
    return Task.FromResult(document);
  }
}