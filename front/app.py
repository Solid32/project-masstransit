import streamlit as st
import pika
import json
import uuid
from datetime import datetime
import time

connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
channel = connection.channel()

# Interface utilisateur Streamlit
st.title('QuotesCollective')
message = st.text_input('Enter votre citation:')
message_data = {
    "Timestamp": datetime.now().isoformat(),
    "Name": message
}
def wait() :
    time.sleep(0)
    return None

if st.button('Envoyer', on_click=wait()):
    channel.basic_publish(exchange='Contracts:IQuoteSubmitted',
                          routing_key='',
                          body=json.dumps(message_data))
    st.success('Message sent successfully')

connection.close()
