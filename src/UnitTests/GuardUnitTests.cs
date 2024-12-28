using Solution.Utils;

namespace UnitTests;

public class GuardUnitTests
{
    [Test]
    public void GuardThat_ConditionIsTrue_DoNotThrow()
    {
        // Arrange
        const bool condition = true;
        
        // Act + Assert
        Assert.That(() => Guard.That(condition, string.Empty), Throws.Nothing);
    }
    
    [Test]
    public void GuardThat_ConditionIsTrue_ThrowInvalidOperationException()
    {
        // Arrange
        const bool condition = false;
        
        // Act + Assert
        Assert.That(() => Guard.That(condition, string.Empty), Throws.InvalidOperationException);
    }
}