def sut IFoo
	def when constructed
		because csut(null)
		should throw : ArgumentNullException
	end
	
	def when calling add
		because Add(1,2)
		should return 3
	end
	
	def when calling subtract
		because Subtract(5,3)
		should return 2
	end
end