namespace TechStore.Domain.Entities;

/// <summary>Базовый класс сущности: даёт всем таблицам единый первичный ключ.</summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
}
