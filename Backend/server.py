import imp
from tornado.httpserver import HTTPServer
from tornado.wsgi import WSGIContainer
from framework import app
from tornado.ioloop import IOLoop

s = HTTPServer(WSGIContainer(app))
s.listen(8001)
IOLoop.current().start()