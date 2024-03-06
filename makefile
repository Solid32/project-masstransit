.DEFAULT_GOAL := default
#################### PACKAGE ACTIONS ###################

stop_rabbitmq: 
				sudo systemctl stop rabbitmq-server
				
				
run_docker:
				docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management                  


install_requirements: 
				pip install streamlit
				pip install pika
				
first_setup:		
				rabbitmqadmin declare exchange name=Contracts:IHelloMessage type=fanout
				rabbitmqadmin declare queue name=GettingStarted
				rabbitmqadmin declare binding source=Contracts:IHelloMessage destination_type=queue destination=GettingStarted routing_key=IHelloContracts
