﻿using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.Domain.Validation;
public class DomainValidation
{
    public static void NotNull(object? target, string fieldName)
    {
        if (target is null)
            throw new EntityValidationException($"{fieldName} should not be null.");
    }

    public static void NotNullOrEmpty(string? target, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(target))
            throw new EntityValidationException($"{fieldName} should not be empty or null.");
    }

    public static void NotNullOrEmpty(Guid? target, string fieldName)
    {
        if (target is null || target.Value == Guid.Empty)
            throw new EntityValidationException($"{fieldName} should not be empty or null.");
    }
}
