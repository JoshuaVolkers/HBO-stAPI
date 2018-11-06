import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator
import json

base_url = ''
users_path = "users"

headers = ''

#Set global API Path
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

#Set global student email    
def test_studentemail(studentemail):
    global studentenemail
    studentenemail = studentemail

#Login and set the bearer token
def test_login_student(studentpass):
    payload = {'emailAddress': studentenemail, 'password': studentpass}
    login_request = requests.post(base_url+users_path+"/login", json=payload)
    f_test_status_code_200(login_request)
    authtoken = login_request.json()
    
    global headers
    headers = {'Authorization': 'Bearer '+authtoken}
    
def test_volgorde():
    f_test_before_add()
    f_test_add_favorite()
    f_test_after_add()
    f_test_delete_favorite()
    f_test_before_add()
 
def f_test_before_add():
    vacancyid = 3
    favorite_request = requests.get(base_url+users_path+"/me/favorites", headers=headers)
    f_test_status_code_200(favorite_request)
    vacancies = favorite_request.json()
    correct = False
    if len(vacancies) > 0:
        for i in range(0, len(vacancies)):
            vacancy = vacancies[i]
            f_test_vacancy(vacancy)
            if vacancy["id"] == vacancyid:
                correct = True
                
    assert correct == False

def f_test_add_favorite():
    vacancyid = 3
    addfavorite_request = requests.put(base_url+users_path+"/me/favorites/"+str(vacancyid), headers=headers)
    f_test_status_code_200(addfavorite_request)
    

def f_test_after_add():
    vacancyid = 3
    favorite_request = requests.get(base_url+users_path+"/me/favorites", headers=headers)
    f_test_status_code_200(favorite_request)
    vacancies = favorite_request.json()
    correct = False
    if len(vacancies) > 0:
        for i in range(0, len(vacancies)):
            vacancy = vacancies[i]
            f_test_vacancy(vacancy)
            if vacancy["id"] == vacancyid:
                correct = True
                
    assert correct == True

def f_test_delete_favorite():
    vacancyid = 3
    deletefavorite_request = requests.delete(base_url+users_path+"/me/favorites/"+str(vacancyid), headers=headers)
    f_test_status_code_200(deletefavorite_request)
    
def f_test_vacancy(vacancy):
    v = Validator(api_schemas.vacancy_schema)
    correct = v.validate(vacancy)
    assert correct == True
    
def f_test_status_code_200(request):
    assert request.status_code == 200  
    