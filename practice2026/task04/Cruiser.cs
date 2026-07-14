namespace task04;

public class Cruiser : ISpaceship
{
    public int Speed => 50;
    public int FirePower => 100;

    public void MoveForward()
    {
        // Крейсер медленно движется вперед
    }

    public void Rotate(int angle)
    {
        // Крейсер поворачивается на заданный угол
    }

    public void Fire()
    {
        // Крейсер стреляет мощной фотонной ракетой
        // test CI
    }
}
