ACCESS_TOKEN="$(curl -v -H "Content-Type: application/x-www-form-urlencoded" -X POST -d 'grant_type=password&username=test@test.com&password=J8cG!FjD' http://localhost:5000/connect/token | jq -r '.access_token')"

echo $ACCESS_TOKEN

# Passing the ACCESS_TOKEN as a Authorization header and quering the list of groceries
curl -v -H "Authorization: Bearer $ACCESS_TOKEN" http://localhost:5000/api/users