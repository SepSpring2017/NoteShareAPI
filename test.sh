# Check that we receive a proper error message for the user registration
curl -v -H "Content-Type: application/json" -d '{Email:"testno",Password:"notlong"}' http://localhost:5000/api/Users

# Generate an access token
ACCESS_TOKEN="$(curl -v -H "Content-Type: application/x-www-form-urlencoded" -d 'grant_type=password&username=test@test.com&password=J8cG!FjD' http://localhost:5000/connect/token | jq -r '.access_token')"

# Passing the ACCESS_TOKEN as an Authorization header and quering the list of users
curl -v -H "Authorization: Bearer $ACCESS_TOKEN" http://localhost:5000/api/users