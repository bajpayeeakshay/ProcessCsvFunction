namespace ProcessCsvFunction.Services.Models;

public class Meta
{
    public GoldenCopy? GoldenCopy { get; set; }
    public Pagination? Pagination { get; set; }
}

public class GoldenCopy
{
    public DateTimeOffset PublishDate { get; set; }
}

public class Pagination
{
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int From { get; set; }
    public int To { get; set; }
    public int Total { get; set; }
    public int LastPage { get; set; }
}

public class Links
{
    public string? First { get; set; }
    public string? Next { get; set; }
    public string? Last { get; set; }
}

public class LegalName
{
    public string? Name { get; set; }
    public string? Language { get; set; }
}

public class OtherName
{
    public string? Name { get; set; }
    public string? Language { get; set; }
    public string? Type { get; set; }
}

public class LegalAddress
{
    public string? Language { get; set; }
    public List<string>? AddressLines { get; set; }
    public string? AddressNumber { get; set; }
    public string? AddressNumberWithinBuilding { get; set; }
    public string? MailRouting { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
}

public class Entity
{
    public LegalName? LegalName { get; set; }
    public List<OtherName>? OtherNames { get; set; }
    public List<string?>? TransliteratedOtherNames { get; set; }
    public LegalAddress? LegalAddress { get; set; }
    public LegalAddress? HeadquartersAddress { get; set; }
    public AuditDetailModel? RegisteredAt { get; set; }
    public string? RegisteredAs { get; set; }
    public string? Jurisdiction { get; set; }
    public string? Category { get; set; }
    public AuditDetailModel? LegalForm { get; set; }
    public EntityModel? AssociatedEntity { get; set; }
    public string? Status { get; set; }
    public Expiration? Expiration { get; set; }
    public EntityModel? SuccessorEntity { get; set; }
    public List<EntityModel>? SuccessorEntities { get; set; }
    public DateTime? CreationDate { get; set; }
    public string? SubCategory { get; set; }
    public List<LegalAddress>? OtherAddresses { get; set; }
    public List<object>? EventGroups { get; set; }
}

public class Registration
{
    public DateTime? InitialRegistrationDate { get; set; }
    public DateTime? LastUpdateDate { get; set; }
    public string? Status { get; set; }
    public DateTime? NextRenewalDate { get; set; }
    public string? ManagingLou { get; set; }
    public string? CorroborationLevel { get; set; }
    public AuditDetailModel? ValidatedAt { get; set; }
    public string? ValidatedAs { get; set; }
    public List<object>? OtherValidationAuthorities { get; set; }
}

public class LeiRecordAttributes
{
    public string? Lei { get; set; }
    public Entity? Entity { get; set; }
    public Registration? Registration { get; set; }
    public List<string>? Bic { get; set; }
    public List<string>? Mic { get; set; }
    public string? Ocid { get; set; }
    public List<string>? Spglobal { get; set; }
}

public class RelatedLinkModel
{
    public string? Related { get; set; }
}

public class Parent
{
    public string? RelationshipRecord { get; set; }
    public string? LeiRecord { get; set; }
}

public class Children
{
    public string? RelationshipRecords { get; set; }
    public string? Related { get; set; }
}

public class Relationships
{
    public RelatedLinkModel? ManagingLou { get; set; }
    public RelatedLinkModel? LeiIssuer { get; set; }
    public RelatedLinkModel? FieldModifications { get; set; }
    public Parent? DirectParent { get; set; }
    public Parent? UltimateParent { get; set; }
    public Children? DirectChildren { get; set; }
}

public class LeiRecord
{
    public string? Type { get; set; }
    public string? Id { get; set; }
    public LeiRecordAttributes? Attributes { get; set; }
    public Relationships? Relationships { get; set; }
    public Links? Links { get; set; }
}

public class LeiRecordRoot
{
    public Meta? Meta { get; set; }
    public Links? Links { get; set; }
    public List<LeiRecord>? Data { get; set; }
}

public class AuditDetailModel
{
    public string? Id { get; set; }
    public string? Other { get; set; }
}

public class Expiration
{
    public DateTime? Date { get; set; }
    public string? Reason { get; set; }
}

public class EntityModel
{
    public string? Lei { get; set; }
    public string? Name { get; set; }
}

public class LeiRecordLinks
{
    public string? Self { get; set; }
}

