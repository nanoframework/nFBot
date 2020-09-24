

### Welcome to the **nanoFramework** nFBot repository!

nFBot is the **nanoFramework** Discord bot used for various operations including FAQ tagging.

## Configuration
nFBot is configured by the `config.json` which is included with default values. You may also refer to the `config.example.json` file in the case that you need to see the original values of elements.

- `prefix` - This property defines the prefix users must use to call commands on the bot. Mentioning the bot is also considered a valid prefix.

- `debug_token` - This does not have to be configured in production environments. When the bot built in Debug mode, this value will be used to connect the bot to Discord. In Release builds, this value will be pulled from the Azure environment as `token`.

- `status_text` - This is the text to display as "Playing" on Discord.

- `welcome_message` - This is the text sent via DM to the user when they join the server.

- `admin_role_id` - This is the Discord Snowflake ID representing the role assigned to users who have access to administrative operations on the bot, such as shutting it down.

- `storage_mode` - This sets what mode to use for storing data. The `debug_storage_connection_string` property in Debug builds and `storage_connection_string` environment variable in Release builds will be used to connect to the specified storage mode, and therefore this value must be defined properly. Valid values for this configuration option are as follows: `mysql`, `mssql`

- `debug_storage_connection_string` - This does not have to be configured in production environments. When the bot built in Debug mode, this value will be used as the connection string for the `storage_mode`. In Release builds, this value will be pulled from the Azure environment as `storage_connection_string`.

## Deployment
The nFBot is designed to be deployed on Azure as a continuous WebJob.

## License

The **nanoFramework** nFBot is licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/)
to clarify expected behavior in our community.
