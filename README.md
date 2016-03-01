# Curse REST Proxy

This is a simple REST API proxy for the Curse web services. Online at
https://curse-rest-proxy.azurewebsites.net/api

Curse uses WCF services in SOAP binary transport mode, which makes communicating with them outside
of a .NET environment very difficult. This proxy provides a thin REST wrapper around those services
to allow other platforms easier access to the Curse client services.

## Usage

### Authentication

Usage of the Curse WCF services requires authentication with a valid username and password.
Authenticating will return a token that can be used on subsequent calls to the services.

To authenticate, send a POST request to `/api/authenticate`.

	POST https://curse-rest-proxy.azurewebsites.net/api/authenticate
	Accept: application/json
	{
	  "username": "<your curse username or email>",
	  "password": "<your password>"
	}

If authentication with Curse is successful, the `/api/authenticate` endpoint will respond with:

	200 Ok
	Content-Type: application/json
	{
	  "session": {
		"actual_premium_status": false,
		"effective_premium_status": false,
		"email_address": "<your curse email>",
		"session_id": "<a session id>",
		"subscription_token": <your subscription token>,
		"token": "<a token>",
		"user_id": <your user id>,
		"username": "<your curse username>"
	  },
	  "status": "Success"
	}

If authentication with Curse fails, the `/api/authenticate` endpoint will respond with:

	401 Unauthorized
	Www-Authenticate: Token realm="api"

If the POST data is missing or malformed, the `/api/authenticate` endpoint will respond with:

	400 Bad request
	Content-Type: application/json
	{
	  "message": "Username and password are required"
	}

Once authenticated, subsequent requests to the service must include the `Authorization` header with
the value of `Token <userid>:<token>` where `userid` and `token` come from the authenticate response.
For example:

	GET https://curse-rest-proxy.azurewebsites.net/api/addon/1
	Content-Type: application/json
	Authorization: Token 12345:your token

If the header is missing, not in the correct form, or the token is rejected by Curse, the API will
respond with:

	401 Unauthorized
	Www-Authenticate: Token realm="api"

The only unauthenticated endpoints are:

	GET https://curse-rest-proxy.azurewebsites.net/api
	GET https://curse-rest-proxy.azurewebsites.net/api/status
	POST https://curse-rest-proxy.azurewebsites.net/api/authenticate

Calls to any other endpoint must include a valid `Authorization` header.

## License

Curse REST Proxy is released under the [MIT license](https://opensource.org/licenses/MIT).