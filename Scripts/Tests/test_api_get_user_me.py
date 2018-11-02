import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator
import json

users_path = "users"


def f_test_user(user):
    v = Validator(api_schemas.user_schema)
    correct = v.validate(user)
    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200


# set global base_url variable
base_url = ''
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

headers = ''

# Login to student
@pytest.mark.unit
def test_login_student(studentemail, studentpass):
    payload = {'emailAddress': studentemail, 'password': studentpass}
    login_request = requests.post(base_url+users_path+"/login", json=payload)
    f_test_status_code_200(login_request)
    token = login_request.json()
    jwt_token = token["authToken"]

    global headers
    headers = {'Authorization': 'Bearer '+jwt_token}

# Test user schema
def test_user_schema():
    user_request = requests.get(base_url+users_path+"/me", headers=headers)
    f_test_status_code_200(user_request)
    user = user_request.json()
    f_test_user(user)
