# Sundial
Sundial is a simple application that can be used to profile code and gives a nice html output of the results.

## What does it profile?
Currently it checks speed, CPU, and memory usage but I plan on opening it up a bit more so that other information can be added/removed depending on your needs.

## How do I set it up?
Well I've uploaded a package to chocolatey but it appears that they're a bit backlogged at the moment. Haven't even received a message from them yet. But this should work:

    choco install sundial -version 0.1.0
    
In theory anyway... From there you should have the app in your path (sundial.exe). Otherwise just download it here and build. Shouldn't take anything special.

## OK, it's there... Now what?
Now you need to create your tasks that you want profiled. In order to do this, create a class library project and pull down the Sundial Core Library package via NuGet:

    Install-Package sundial.core -Pre
    
Since this app is still in beta you're dealing with prerelease versions of stuff... At least for now. From there you just need to create a class that inherits from ITimedTask for each item that you want to time. So for instance:

    /// <summary>
    /// Example task 1
    /// </summary>
    public class ExampleTask1 : ITimedTask
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get { return "Example1"; } }
        
        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            System.Random Rand = new Random();
            int[] Temp = new int[10000];
            for (int x = 0; x < Temp.Length; ++x)
            {
                Temp[x] = Rand.Next();
            }
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
    
You'll notice that it inherits from IDisposable. The reason for this is so that you can do any necessary setup in the constructor and any teardown that is required in the Dispose function. Note that the constructor is only called once at the application's start up and the Dispose function is only called once as well when the application is shutting down. From there, just build the DLL.

## How do I run this thing?
Go to the location of the class library that you built before and run the following:

    sundial
    
That's it. It will then run your set of tasks and put the results in a directory called Results. Also note that the application, by default, runs each task a total of 10,000 times. So for long running tasks, you may wish to go into the app.config file for sundial.exe and modify the NumberOfIterations or OutputDirectory if you would like the results in a different directory. In the future I'll add command line arguments, but you know... Beta... Also, in case you've used the chocolatey installer, it will add the application here:

    C:\ProgramData\chocolatey\lib\sundial
    
As I work on this a bit more, I'll add examples of the output, etc. And if you run into issues, have feature requests, etc. please post them here. And as I've said before, this is beta so don't be shocked if things change a bit as I work out how I want this thing to function.
