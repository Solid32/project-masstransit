import streamlit as st
import pika
import json
import uuid
from datetime import datetime

connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
channel = connection.channel()

# Interface utilisateur Streamlit
st.title('QuotesCollective')
message = st.text_input('Enter votre citation:')
message_data = {
    "Id": str(uuid.uuid4()),
    "Timestamp": datetime.now().isoformat(),
    "Name": message
}

if st.button('Envoyer'):
    channel.basic_publish(exchange='Contracts:IHelloMessage',
                          routing_key='',
                          body=json.dumps(message_data))
    st.success('Message sent successfully')

connection.close()
