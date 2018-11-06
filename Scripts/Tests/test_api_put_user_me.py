import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator
import json

def f_test_user(user):
    v = Validator(api_schemas.user_schema)
    correct = v.validate(user)
    assert correct == True

def f_test_user_input(user, educationLevelId, majorId, preferredLanguageId, countryId, city):
    correct = True
    if user["educationLevelId"] != educationLevelId:
        correct = False

    if user["majorId"] != majorId:
        correct = False

    if user["preferredLanguageId"] != preferredLanguageId:
        correct = False

    if user["location"]["countryId"] != countryId:
        correct = False

    if user["location"]["city"] != city:
        correct = False

    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200



base_url = ''
users_path = "users"

# set global base_url variable
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

    global headers
    headers = {'Authorization': 'Bearer '+token}

# Test user update
def test_user_update():
    educationLevelId = 1
    majorId = 1
    preferredLanguageId = 1
    countryId = 1
    city = 'Haarlem'
    additionalLocationIdentifier = 'Noord Holland'

    payload = {
        'educationLevelId': educationLevelId, 
        'majorId': majorId,
        'preferredLanguageId': preferredLanguageId,
        'countryId': countryId,
        'city': city,
        'additionalLocationIdentifier': additionalLocationIdentifier
        }
    user_request = requests.put(base_url+users_path+"/me", headers=headers, json=payload)
    f_test_status_code_200(user_request)
    user = user_request.json()
    f_test_user(user)
    f_test_user_input(user, educationLevelId, majorId, preferredLanguageId, countryId, city)
