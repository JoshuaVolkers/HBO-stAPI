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

def f_test_max_users(users, max):
    correct = True
    if len(users) > max:
        correct = False

    assert correct == True

def f_test_educational_attainments_filter(users, educationalAttainmentIds):
    for i in range(0, len(users)):
        user = users[i]
        correct = False

        for j in range(0, len(educationalAttainmentIds)):
            if user["educationalAttainmentId"] == educationalAttainmentIds[j]:
                correct = True
                break

        assert correct == True

def f_test_educations_filter(users, educationIds):
    for i in range(0, len(users)):
        user = users[i]
        correct = False

        for j in range(0, len(educationIds)):
            if user["educationId"] == educationIds[j]:
                correct = True
                break

        assert correct == True

def f_test_country_name(users, country_name):
    for i in range(0, len(users)):
        user = users[i]
        correct = True
        if user["location"]["countryName"] != country_name:
            correct = False

        assert correct == True

def f_test_city_name(users, city_name):
    for i in range(0, len(users)):
        user = users[i]
        correct = True
        if user["location"]["city"] != city_name:
            correct = False

        assert correct == True

def f_test_location_range(users, r):
    for i in range(0, len(users)):
        user = users[i]
        correct = True
        if user["location"]["range"] > r:
            correct = False

        assert correct == True

def f_test_preferred_language(users, lanuage_id):
    for i in range(0, len(users)):
        user = users[i]
        correct = True
        if user["preferredLanguageId"] != lanuage_id:
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

# global headers variable
headers = ''

# Login to admin
@pytest.mark.unit
def test_admin_login(adminemail, adminpass):
    payload = {'emailAddress': adminemail, 'password': adminpass}
    login_request = requests.post(base_url+users_path+"/login", json=payload)
    f_test_status_code_200(login_request)
    token = login_request.json()

    global headers
    headers = {'Authorization': 'Bearer '+token}

# Test users schema
def test_users_schema():
    users_request = requests.get(base_url+users_path, headers=headers)
    f_test_status_code_200(users_request)
    users = users_request.json()
    f_test_max_users(users, 5)
    f_test_user(users[0])

# Test users (educationalAttainments = 1,2)
def test_users_educationalattainments():
    users_request = requests.get(base_url+users_path+"?educationalAttainments=1,2", headers=headers)
    f_test_status_code_200(users_request)
    users = users_request.json()
    f_test_educational_attainments_filter(users, [1,2])

# Test users (educations = 1,2)
def test_users_educations():
    users_request = requests.get(base_url+users_path+"?educations=1,2", headers=headers)
    f_test_status_code_200(users_request)
    users = users_request.json()
    f_test_educations_filter(users, [1,2])

# Test users (cityName = Haarlem)
def test_users_city():
    users_request = requests.get(base_url+users_path+"?cityName=Haarlem", headers=headers)
    f_test_status_code_200(users_request)
    users = users_request.json()
    f_test_city_name(users, "Haarlem")

# Test users (countryName = Nederland)
def test_users_country():
    users_request = requests.get(base_url+users_path+"?countryName=Nederland", headers=headers)
    f_test_status_code_200(users_request)
    users = users_request.json()
    f_test_country_name(users, "Nederland")

# Test users (preferredLanguage = 1)
def test_users_language():
    users_request = requests.get(base_url+users_path+"?preferredLanguage=1", headers=headers)
    f_test_status_code_200(users_request)
    users = users_request.json()
    f_test_preferred_language(users, 1)