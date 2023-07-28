SerialPort is only on Window and some standalone platform.

Meaning that you won't have the System.io.port library.
But we need to be able to use at least the some enum and abstract library to prepare the work that the native code will use to speak with the platform.