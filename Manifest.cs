using Core.Modules.Manifest;
using Summary.Bale;

[assembly: Feature(
    Id = Bale.Feature.Bale,
    Name = "پیام رسان بله",
    Description = "مجموعه‌ای از رویداد و تسک‌ها جهت ارتباط با سامانه پیام رسان بله.",
    Category = Bale.Public.Category,
    Dependencies = new[] { "Core.Workflows" }
)]