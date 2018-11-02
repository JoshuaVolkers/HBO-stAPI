import api_schemas
import requests
import cerberus
import pytest
import uuid
from cerberus import Validator
import json

users_path = "users"

# arrange tests
def f_test_login(token):
    correct = isinstance(token, str)
    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200

def f_test_status_code_401(request):
    assert request.status_code == 401

# set global base_url variable
base_url = ''
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

# login with valid credentials
@pytest.mark.unit
def test_login_student(studentemail, studentpass):
    payload = {'emailAddress': studentemail, 'password': studentpass}
    login_request = requests.post(base_url+users_path+"/login", json=payload)
    f_test_status_code_200(login_request)
    token = login_request.json()
    f_test_login(token)

# login with invalid credentials
def test_login_student_fail(studentemail):
    payload = {'emailAddress': studentemail, 'password': str(uuid.uuid4())}
    login_request = requests.post(base_url+users_path+"/login", json=payload)
    f_test_status_code_401(login_request)