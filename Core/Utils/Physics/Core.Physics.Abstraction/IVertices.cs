namespace Core.Physics.Abstraction;

public interface IVertices<T>
{
    T this[int index] { get; set; }
    void Add(T vertex);
    T GetCentroid();
    void Translate(T value);
    void Scale(T value);
    void Rotate(float radians);
    // Outros métodos necessários
}
