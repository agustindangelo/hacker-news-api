# Hacker News API Assessment
## Overview
The **Hacker News API** provides access to items, top stories, and user information. This API follows RESTful principles and allows fetching specific items, searching for items by title, retrieving top stories, and getting user details. It implements caching for improved performance and scalability.

## Endpoints

### **1. Get Item by ID**
**Endpoint:** `GET /api/Item/{id}`

Retrieves details of a specific item by its ID.

#### **Request Parameters:**
| Parameter | Type    | In   | Required | Description |
|-----------|--------|------|----------|-------------|
| `id`      | integer | path | ✅ Yes    | The unique identifier of the item. |

#### **Response:**
- **200 OK** – Successfully retrieves the item.
- **404 Not Found** – If the item does not exist.

**Example Request:**
```sh
GET /api/Item/123
```

---
### **2. Search Items by Title**
**Endpoint:** `GET /api/Item/search`

Searches for items based on their title.

#### **Query Parameters:**
| Parameter | Type   | In    | Required | Description |
|-----------|--------|------|----------|-------------|
| `title`   | string | query | ❌ No     | The title or partial title of the item. |

#### **Response:**
- **200 OK** – Returns a list of items matching the search criteria.

**Example Request:**
```sh
GET /api/Item/search?title=Angular
```

---
### **3. Get Top Stories**
**Endpoint:** `GET /api/topstories`

Retrieves the latest top stories from Hacker News.

#### **Response:**
- **200 OK** – Successfully retrieves the top stories.

**Example Request:**
```sh
GET /api/topstories
```

---
### **4. Get User by Username**
**Endpoint:** `GET /api/User/{username}`

Fetches details about a specific user.

#### **Request Parameters:**
| Parameter   | Type   | In   | Required | Description |
|-------------|--------|------|----------|-------------|
| `username`  | string | path | ✅ Yes    | The username of the user. |

#### **Response:**
- **200 OK** – Returns user details.
- **404 Not Found** – If the user does not exist.

**Example Request:**
```sh
GET /api/User/alice
```

---
## **Authentication & Rate Limits**
This API does not currently require authentication. Future updates may include API keys or OAuth authentication.

## **Error Handling**
The API returns standard HTTP response codes:
- **200 OK** – Request successful.
- **400 Bad Request** – Invalid request parameters.
- **404 Not Found** – The requested resource was not found.
- **500 Internal Server Error** – Unexpected server error.

## **Usage Notes**
- All responses are returned in **JSON format**.
- Query parameters are case-sensitive.
