using AutoFixture.Kernel;

public class MemoryStreamSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(Stream))
        {
            // Return a predefined MemoryStream instance or null
            return new MemoryStream(new byte[] { 0x01, 0x02, 0x03 });
        }

        return new NoSpecimen();
    }
}