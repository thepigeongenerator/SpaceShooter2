global using Microsoft.Xna.Framework;

//TODO: add sounds + music?
internal class Program {
    //entry point of the application, called by OS
    private static int Main() {
        //create an instance of the game, make sure that it is disposed correctly once finished
        using var game = new SpaceShooter2.SpaceShooter();
        game.Run(); //execute the game

        //0; successful execution
        return 0;
    }
}
