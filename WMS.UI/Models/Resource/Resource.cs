using WMS.UI.Models.Enums;

namespace WMS.UI.Models.Resource;

public record Resource(Guid Id, string Title, State State);