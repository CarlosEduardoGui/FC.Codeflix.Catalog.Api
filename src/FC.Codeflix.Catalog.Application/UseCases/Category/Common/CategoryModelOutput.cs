﻿using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
namespace FC.Codeflix.Catalog.Application.UseCases.Category.Common;
public class CategoryModelOutput
{
    public CategoryModelOutput(
        Guid id,
        string? name,
        string? description,
        DateTime createdAt,
        bool isActive
    )
    {
        Id = id;
        Name = name!;
        Description = description!;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public static CategoryModelOutput FromCategory(DomainEntity.Category category)
        => new(
            category.Id,
            category.Name,
            category.Description,
            category.CreatedAt,
            category.IsActive
        );
}
