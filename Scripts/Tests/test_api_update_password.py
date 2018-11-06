import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator
import json

base_url = ''
users_path = "users"

headers = ''

studentenemail = ''

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

#Change the password    
def test_update_password(studentpass):
    oldpassword = studentpass
    newpassword = 'test1234'
    
    payload = {'oldPassword': oldpassword, 'newPassword': newpassword}
    passwordupdate_request = requests.put(base_url+users_path+"/me/updatepassword", headers=headers,json=payload)
    f_test_status_code_200(passwordupdate_request)
    f_test_new_password(newpassword)
    f_reset_newpassword(oldpassword, newpassword)

#Test the new password    
def f_test_new_password(newpassword):
    payload = {'emailAddress': studentenemail, 'password': newpassword}
    login_request = requests.post(base_url+users_path+"/login", json=payload)
    correct = False
    if(login_request.status_code == 200):
        correct = True
        
    assert correct == True    
               
def f_test_status_code_200(request):
    assert request.status_code == 200

#Reset the password after the test    
def f_reset_newpassword(oldpassword, newpassword):
    payload = {'oldPassword': newpassword, 'newPassword': oldpassword}
    passwordupdate_request = requests.put(base_url+users_path+"/me/updatepassword", headers=headers,json=payload)
    f_test_status_code_200(passwordupdate_request)   
    