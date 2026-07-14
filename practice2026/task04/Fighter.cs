namespace task04;

public class Fighter : ISpaceship
{
    public int Speed => 100;
    public int FirePower => 30;

    public void MoveForward()
    {
        // Истребитель быстро движется вперед
    }

    public void Rotate(int angle)
    {
        // Истребитель поворачивается на заданный угол
    }

    public void Fire()
    {
        // Истребитель стреляет слабой фотонной ракетой
    }
}
