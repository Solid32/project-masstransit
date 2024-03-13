import streamlit as st
import pika
import json
from datetime import datetime
import time

connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
channel = connection.channel()
def clear_text():
    st.session_state.my_text = st.session_state.widget
    st.session_state.widget = ""
# Interface utilisateur Streamlit
st.title('QuotesCollective')
st.text_input('Enter votre citation:', key = 'widget', on_change = clear_text)
message = st.session_state.get('my_text', '')
message_data = {
    "Timestamp": datetime.now().isoformat(),
    "Name": message
}
def wait() :
    return None

if st.button('Envoyer', on_click=wait()):
    channel.basic_publish(exchange='Contracts:IQuoteSubmitted',
                          routing_key='',
                          body=json.dumps(message_data))
    st.success('Message sent successfully')


connection.close()
