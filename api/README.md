# Api Documentation

---

## 1. **API Endpoint: `GET /api/events/search`**

### **Endpoint Details**

| **Method** | **Endpoint**         | **Description**                              |
|------------|----------------------|----------------------------------------------|
| `GET`      | `/api/events/search` | Search and retrieve paginated, sorted events |

### **Query Parameters**

| **Parameter** | **Type** | **Required** | **Default** | **Description**                                      |
|---------------|----------|--------------|-------------|------------------------------------------------------|
| `page`        | `int`    | Yes          | `1`         | The page number to retrieve.                         |
| `size`        | `int`    | Yes          | `10`        | Number of events per page.                           |
| `search`      | `string` | No           | `null`      | Search term for filtering event titles/descriptions. |
| `sort`        | `string` | No           | `"date"`    | Sorting field (`"title"` or `"date"`).               |
| `order`       | `string` | No           | `"asc"`     | Sorting order (`"asc"` or `"desc"`).                 |

---

### **Example API Requests**

#### **Basic Pagination (First Page, Default Sorting)**

```
GET /api/events/search?page=1&size=10
```

**Expected Response:**

```json
{
  "value": [
    {
      "id": "0c576588-5a9f-4ebb-a811-7ac9415faa7f",
      "title": "AI & Future Technology Summit",
      "description": "<p>Join industry leaders and innovators...</p>",
      "location": "San Francisco, CA",
      "startTime": "2125-02-09T21:53:47.575770Z",
      "endTime": "2125-02-10T02:53:47.575782Z"
    }
  ],
  "status": "Ok",
  "totalItems": 100,
  "pageSize": 10,
  "currentPage": 1,
  "totalPages": 10,
  "errors": []
}
```

---

#### **Searching for an Event (`search=AI`)**

```
GET /api/events/search?page=1&size=10&search=AI
```

**Expected Response (Highlighted Search Term)**

```json
{
  "value": [
    {
      "id": "0c576588-5a9f-4ebb-a811-7ac9415faa7f",
      "title": "<pre>AI</pre> & Future Technology Summit",
      "description": "<p>Join industry leaders and innovators in <pre>AI</pre>...</p>",
      "location": "San Francisco, CA",
      "startTime": "2125-02-09T21:53:47.575770Z",
      "endTime": "2125-02-10T02:53:47.575782Z"
    }
  ],
  "status": "Ok",
  "totalItems": 5,
  "pageSize": 10,
  "currentPage": 1,
  "totalPages": 1,
  "errors": []
}
```

---

#### **Sorting by Title (Descending)**

```
GET /api/events/search?page=1&size=10&sort=title&order=desc
```

**🔹 Expected Response:**

Events are sorted **by title in descending order**.

---

#### **Invalid Page Number (Too High)**

```
GET /api/events/search?page=100&size=10
```

**Expected Response (Error Handling)**

```json
{
  "status": "Invalid",
  "errors": ["Page number is too high."]
}
```


## 2. **API Endpoint: `POST /api/rsvps/{eventId}`**

### **Endpoint Details**

| **Method** | **Endpoint**             | **Description**                         |
|------------|--------------------------|-----------------------------------------|
| `POST`     | `/api/rsvps/{eventId}`    | Submit an RSVP for a specific event    |

### **Request Body Parameters**

| **Parameter** | **Type**   | **Required** | **Description**                                     |
|--------------|-----------|--------------|-----------------------------------------------------|
| `firstName`  | `string`  | Yes          | First name of the attendee.                        |
| `lastName`   | `string`  | Yes          | Last name of the attendee.                         |
| `email`      | `string`  | Yes          | Email address of the attendee.                     |

### **Response Codes & Handling**

| **Status Code** | **Description**                                         |
|----------------|---------------------------------------------------------|
| `201 Created`  | RSVP successfully submitted. Returns RSVP ID.           |
| `400 Bad Request` | Invalid input fields.                                 |
| `409 Conflict` | Attendee with the same email is already registered.     |

---

### **Example API Request**

#### **Submit RSVP for an Event**
```
POST /api/rsvps/0c576588-5a9f-4ebb-a811-7ac9415faa7f
Content-Type: application/json
```

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com"
}
```

**Expected Response (Success `201 Created`):**
```json
{
  "value": "dce3d49b-6e2b-4b9e-bad5-1a9d7e8a9f2c",
  "status": "Ok",
  "errors": []
}
```

---

### **Handling RSVP Conflict (`409 Conflict`)**
If the attendee has already RSVP’d for the event, a conflict error is returned.

#### **Example Conflict Response**
```json
{
  "title": "There was a conflict.",
  "status": 409,
  "detail": "Next error(s) occurred: * Attendee with this email is already registered for this event."
}
```

---
