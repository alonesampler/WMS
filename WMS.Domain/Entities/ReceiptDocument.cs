using MWS.Domain.Exceptions;

namespace WMS.Domain.Entities;

public class ReceiptDocument
{
    private ReceiptDocument() { }

    private ReceiptDocument(Guid id, string applicationNumber, DateTime date)
    {
        Id = id;
        ApplicationNumber = applicationNumber;
        Date = date;
    }
    
    public Guid Id { get; private set; }
    
    public string ApplicationNumber { get; private set; }
    
    public DateTime Date { get; private set; }

    public List<ReceiptItem> Items { get; private set; } = new();

    public static ReceiptDocument Create(Guid id, string applicationNumber, DateTime date)
    {
        if (id == Guid.Empty)
            throw new DomainException("Guid is empty");
        
        if(string.IsNullOrWhiteSpace(applicationNumber))
            throw new DomainException("Title can not be empty or writespace");
        
        return new ReceiptDocument(id, applicationNumber, date);
    }

    public void Update(string applicationNumber, DateTime date)
    {
        if(applicationNumber != ApplicationNumber)
            ApplicationNumber = applicationNumber;
        
        if(date != Date)
            Date = date;
    }
}