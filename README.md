
# Social Sphere Web API

Welcome to the Social Sphere Web API! This API serves as the backend for the Social Sphere application, providing features for user authentication, posting tweets, viewing and liking tweets, and commenting on tweets. It's built using .NET Core and uses MongoDB for data storage. User authentication is implemented using JWT Bearer tokens, and API versioning is incorporated for better management. Detailed documentation is available through Swagger.

## Features

1. **User Authentication**: Users can authenticate themselves securely using JWT Bearer tokens.

2. **Tweet Management**: Users can create and post tweets, view tweets, and like other users' tweets.

3. **Commenting**: Users have the ability to comment on tweets, fostering engagement and discussion.

4. **MongoDB Integration**: The API leverages MongoDB for data storage, ensuring efficient and scalable data management.

5. **API Versioning**: API versioning is implemented for better control and maintenance as the project evolves.

6. **Swagger Documentation**: Detailed API documentation is available through Swagger, making it easy for developers to understand and utilize the API endpoints.

## Installation and Usage

To set up and use the Social Sphere Web API, follow these steps:

1. **Prerequisites**: Ensure you have .NET Core and MongoDB installed on your system.

2. **Clone the Repository**: Clone this repository to your local machine.

   ```bash
   git clone https://github.com/your-repo-url.git
   ```

3. **Configuration**: Update the app settings to configure your MongoDB connection and JWT settings.

4. **Build and Run**: Build the project and run it.

   ```bash
   dotnet build
   dotnet run
   ```

5. **API Documentation**: Access the Swagger documentation by navigating to `http://localhost:5000/swagger` in your web browser. Here, you can explore the available endpoints and make API requests.

6. **Testing**: Utilize the API endpoints as needed for your application. Ensure you have proper authentication using JWT tokens.

## API Endpoints

Here are some of the key API endpoints available:

- `/api/authenticate`: Endpoint for user authentication.
- `/api/tweets`: Endpoint for managing tweets (posting, viewing, and liking).
- `/api/comments`: Endpoint for commenting on tweets.

Please refer to the Swagger documentation for a complete list of available endpoints, request, and response examples.

## Contributing

If you'd like to contribute to this project, please follow these steps:

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Make your changes and submit a pull request.

## License

This project is licensed under the [Your License Name] License - see the [LICENSE](LICENSE) file for details.

## Contact

If you have any questions or need further assistance, feel free to contact us at [your email address].

Happy coding!
