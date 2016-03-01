# Curse REST Proxy

This is a simple REST API proxy for the Curse web services. Online at
https://curse-rest-proxy.azurewebsites.net/api

Curse uses WCF services in SOAP binary transport mode, which makes communicating with them outside
of a .NET environment very difficult. This proxy provides a thin REST wrapper around those services
to allow other platforms easier access to the Curse client services.

## Overview

| Name                              | Endpoint                                                                                   | Description |
|-----------------------------------|--------------------------------------------------------------------------------------------|-------------|
| Home                              | GET https://curse-rest-proxy.azurewebsites.net/api                                         |             |
| Status                            | GET https://curse-rest-proxy.azurewebsites.net/api/status                                  | Service Status |
| [Authentication](#authentication) | POST https://curse-rest-proxy.azurewebsites.net/api/authenticate                           | Authentication |
| [Get Add On](#get-add-on)          | GET https://curse-rest-proxy.azurewebsites.net/api/addon/:id                               | Get the details for an add on |
| Get addon description             | GET https://curse-rest-proxy.azurewebsites.net/api/addon/:id/description                   | Get the description of an addon |
| Get addon files                   | GET https://curse-rest-proxy.azurewebsites.net/api/addon/:id/files                         | Get the list of files for this addon |
| Get addon file                    | GET https://curse-rest-proxy.azurewebsites.net/api/addon/:addon_id/file/:file_id           | Get the details for a file |
| Get addon file changelog          | GET https://curse-rest-proxy.azurewebsites.net/api/addon/:addon_id/file/:file_id/changelog | Get the changelog for a file |

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

### Get Add On

Get the details for an add on.

#### Request

	GET https://curse-rest-proxy.azurewebsites.net/api/addon/1
	Accept: application/json

#### Response

	200 Ok
	Content-Type: application/json
	{
	  "id": 1,
	  "name": "Package Name",
	  "package_type": "ModPack", // ModPack | Mod | World
	  "summary": "A demo package",
	  "web_site_url": "http://www.curse.com/modpacks/minecraft/1-package-name"
	  "attachments": [
		{
		  "description": "",
		  "is_default": true,
		  "thumbnail_url": "http://example.com/path/to/thumbnail.png",
		  "title": "title.png",
		  "url": "http://example.com/path/to/file.png"
		}
	  ],
	  "authors": [
		{
		  "name": "John Doe",
		  "url": "http://www.curse.com/members/JohnDoe.aspx"
		}
	  ],
	  "avatar_url": null,
	  "categories": [
		{
		  "id": 4475,
		  "name": "Adventure and RPG",
		  "url": "http://www.curse.com/modpacks/minecraft/category/adventure-and-rpg"
		},
		{
		  "id": 4479,
		  "name": "Hardcore",
		  "url": "http://www.curse.com/modpacks/minecraft/category/hardcore"
		}
	  ],
	  "category_section": {
		"extra_include_pattern": null,
		"game_id": 432,
		"id": 4471,
		"initial_inclusion_pattern": ".",
		"name": "Modpacks",
		"package_type": "ModPack",
		"path": "downloads"
	  },
	  "comment_count": 0,
	  "default_file_id": 42,
	  "donation_url": null,
	  "download_count": 101,
	  "external_url": null,
	  "game_id": 432,
	  "icon_id": 1,
	  "install_count": 0,
	  "is_featured": 0,
	  "latest_files": [
		{
		  "alternate_file_id": 0,
		  "dependencies": [],
		  "download_url": "http://example.com/path/to/file-1.0.0.zip",
		  "file_date": "2015-10-07T20:36:26",
		  "file_name": "file-1.0.0.zip",
		  "file_name_on_disk": "file-1.0.0.zip",
		  "file_status": "Normal",
		  "game_version": [
			"1.7.10"
		  ],
		  "id": 42,
		  "is_alternate": false,
		  "is_available": true,
		  "modules": [
			{
			  "fingerprint": 9876543421,
			  "foldername": "manifest.json"
			}
		  ],
		  "package_fingerprint": 987654321,
		  "release_type": "Release"
		}
	  ],
	  "game_version_latest_files": [
		{
		  "file_type": "Release",
		  "game_vesion": "1.7.10",
		  "project_file_id": 42,
		  "project_file_name": "file-1.0.0.zip"
		}
	  ],
	  "likes": 2,
	  "popularity_score": 100.4567,
	  "primary_author_name": "John Doe",
	  "primary_category_avatar_url": null,
	  "primary_category_id": 4475,
	  "primary_category_name": null,
	  "rating": 0,
	  "stage": "Release",
	  "status": "Normal",
	}

## License

Curse REST Proxy is released under the [MIT license](https://opensource.org/licenses/MIT).