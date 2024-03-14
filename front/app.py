import streamlit as st
import pika
import json
from datetime import datetime
import time

connection = pika.BlockingConnection(pika.ConnectionParameters('localhost')) #Pika => Connection à un host RabbitMQ
channel = connection.channel()

# Interface utilisateur Streamlit

def clear_text():
    """Clear text after click send button"""
    st.session_state.my_text = st.session_state.widget
    st.session_state.widget = ""

# Interface envoie du message

st.title('QuotesCollective')
st.text_input('Enter votre citation:', key = 'widget', on_change = clear_text)

#Contenu du message envoyé

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
    st.success(f'Message: {message} envoyé avec succés') # => envoi et formatte en JSON les messages à l'exchange RabbitMQ


connection.close()
