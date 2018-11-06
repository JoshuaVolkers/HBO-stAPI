user_location_schema = {
                    'countryName' : {'type' : 'string', 'nullable' : True},
                    'countryId' : {'type' : 'integer'},
                    'city' : {'type' : 'string', 'nullable' : True},
                    'longitude' : {'type' : 'float'},
                    'latitude' : {'type' : 'float'}
                    }

location_schema = {
                    'zipCode' : {'type' : 'string'},
                    'street' : {'type' : 'string'},
                    'houseNumber' : {'type' : 'integer'},
                    'houseNumberAdditions' : {'type' : 'string'},
                    'range' : {'type' : 'float'},
                    'countryName' : {'type' : 'string', 'nullable' : True},
                    'countryId' : {'type' : 'integer'},
                    'city' : {'type' : 'string'},
                    'longitude' : {'type' : 'float'},
                    'latitude' : {'type' : 'float'}
                    }

user_schema = {
                'id' : {'type' : 'integer'},
                'niceName' : {'type' : 'string'},
                'emailAddress' : {'type' : 'string'},
                'role' : {'type' : 'integer'},
                'active' : {'type' : 'boolean'},
                'educationalAttainmentId' : {'type' : 'integer'},
                'educationalAttainment' : {'type' : 'string', 'nullable' : True},
                'educationId' : {'type' : 'integer'},
                'education' : {'type' : 'string', 'nullable' : True},
                'favoriteVacancies' : {'type' : 'list', 'schema' : {'type' : 'integer'}, 'nullable' : True},
                'reviews' : {'type' : 'list', 'schema' : {'type' : 'integer'}, 'nullable' : True},
                'preferredLanguageId' : {'type' : 'integer'},
                'preferredLanguage' : {'type' : 'string', 'nullable' : True},
                'location' : {'type' : 'dict', 'schema' : user_location_schema}
                }

company_schema = {
                    'description' : {'type' : 'string'}, 
                    'emailAddress' : {'type' : 'string'},
                    'website' : {'type' : 'string'}, 
                    'socialLinkLinkedin' : {'type' : 'string'}, 
                    'id' : {'type' : 'integer'}, 
                    'name' : {'type' : 'string'}, 
                    'logoPath' : {'type' : 'string'}, 
                    'averageReviewStars' : {'type' : 'float'},
                    'majors' : {'type' : 'string', 'nullable' : True},
                    'location' : {'type' : 'dict', 'schema' : location_schema}
                    }

base_company_schema = {
                    'id' : {'type' : 'integer'}, 
                    'name' : {'type' : 'string'}, 
                    'logoPath' : {'type' : 'string'}, 
                    'averageReviewStars' : {'type' : 'float'},
                    'majors' : {'type' : 'string', 'nullable' : True},
                    'location' : {'type' : 'dict', 'schema' : location_schema}
                    }

company_review_schema = {
                    'id' : {'type' : 'integer'},
                    'companyId' : {'type' : 'integer'},
                    'stars' : {'type' : 'integer'},
                    'writtenReview' : {'type' : 'string'},
                    'nameReviewer' : {'type' : 'string'},
                    'reviewDate' : {'type' : 'string'}
                }

review_schema = {
                    'id' : {'type' : 'integer'},
                    'companyId' : {'type' : 'integer'},
					'userId' : {'type' : 'integer'},
                    'stars' : {'type' : 'integer'},
                    'writtenReview' : {'type' : 'string'},
					'anonymous' : {'type' : 'integer'},
                    'creationDate' : {'type' : 'string'},
                    'verifiedReview' : {'type' : 'integer'},
					'verifiedBy' : {'type' : 'integer'},
					'verificationDate' : {'type' : 'string'},
					'locked' : {'type' : 'boolean'}
                }

major_schema = {
                    'id' : {'type' : 'integer'},
                    'crohoNumber' : {'type' : 'integer'},
					'name' : {'type' : 'string'},
                    'educationLevel' : {'type' : 'string'},
					'active' : {'type' : 'integer'}
                }

educationLevel_schema = {
                    'id' : {'type' : 'integer'},
					'name' : {'type' : 'string'}
                }

allowedEmailadress_schema = {
                    'id' : {'type' : 'integer'},
					'emailAddress' : {'type' : 'string'},
					'educationalInstitutionId' : {'type' : 'integer'}
                }

language_schema = {
                    'id' : {'type' : 'integer'},
					'languageName' : {'type' : 'string'},
					'languageIso' : {'type' : 'string'}
                }

internshipType_schema = {
                    'id' : {'type' : 'integer'},
					'name' : {'type' : 'string'}
                }
				
vacancy_schema = {
                    'id' : {'type' : 'integer'}, 
                    'title' : {'type' : 'string'},
                    'creationDate' : {'type' : 'string'}, 
                    'education' : {'type' : 'string'}, 
                    'description' : {'type' : 'string'}, 
                    'companyId' : {'type' : 'integer'}, 
                    'language' : {'type' : 'string'}, 
                    'closingDate' : {'type' : 'string'},
                    'link' : {'type' : 'string'},
                    'educationalAttainment' : {'type' : 'string'},
                    'internshipType' : {'type' : 'string'},
                    'location' : {'type' : 'dict', 'schema' : location_schema},
                    }

country_schema = {
                    'id' : {'type' : 'integer'},
                    'name' : {'type' : 'string'}
                    }